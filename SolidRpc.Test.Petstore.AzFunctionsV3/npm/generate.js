const fs = require('fs')
const request = require('request')
const unzipper = require('unzipper')

const download = (url, path, callback) => {
    var options = {
        url: url,
        headers: {
            'securitykey': 'sdfasfdsaf'
        }
    };
    request(options)
        .pipe(fs.createWriteStream(path))
        .on('close', callback)
}
const packages ='SolidRpc,SolidRpc.Abstractions'
const url = 'http://localhost:7071/front/SolidRpc/Abstractions/Services/Code/INpmGenerator/CreateNpmZip/'+packages
const path = 'generated';
const zipFile = path+'/tmp.zip';

console.log('Downloading package...')
download(url, zipFile, () => {
    console.log('Extracting zip...');
    fs.createReadStream(zipFile)
        .pipe(unzipper.Extract({ path: path }))
        .on('close', () => {
            console.log('...Done!')
            fs.unlinkSync(zipFile)
        });
});
