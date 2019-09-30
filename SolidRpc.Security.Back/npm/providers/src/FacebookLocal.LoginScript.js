import { Security } from 'solidrpc-security';
import { onLoggedIn } from './SolidRpcSecurity.LoginScript';

window.fbAsyncInit = function () {
    FB.init({
      appId      : '{your-app-id}',
      cookie     : true,
      xfbml      : true,
      version    : '{api-version}'
    });
      
    FB.AppEvents.logPageView();   

    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
      
  };

function statusChangeCallback(response) {
    
    if (response.status === 'connected') {
        var svc = new Security.Services.Facebook.FacebookLocalImpl();
        svc.LoggedIn(response.authResponse.accessToken).subscribe(accessToken => {
            onLoggedIn(accessToken);
        });
    }
    else {
        console.log('facebook status:' + response.status);
    }
}

function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}
  (function(d, s, id){
     var js, fjs = d.getElementsByTagName(s)[0];
     if (d.getElementById(id)) {return;}
     js = d.createElement(s); js.id = id;
     js.src = "https://connect.facebook.net/en_US/sdk.js";
     fjs.parentNode.insertBefore(js, fjs);
   }(document, 'script', 'facebook-jssdk'));


window.solidRpcDoFacebookLogin = function () {
    console.log('logging in to facebook');
    FB.login(statusChangeCallback);
};

window.solidRpcDoFacebookLogout = function (accessToken) {
    console.log('logging out from facebook');
};