  window.fbAsyncInit = function() {
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

var ns = SolidRpc.declareNamespace('SolidRpc.Security.Login.Facebook');

function statusChangeCallback(response) {
    
    if (response.status === 'connected') {
        window.SolidRpc.Security.doLogin('Facebook', '{loggedin-postback}'.replace('{accessToken}', response.authResponse.accessToken));
    }
    else {
        console.log('facebook staus:' + response.status);
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


function solidRpcDoFacebookLogin() {
    console.log('logging in to facebook');
    FB.login(statusChangeCallback);
}

function solidRpcDoFacebookLogout(accessToken) {
    console.log('logging out from facebook');
}