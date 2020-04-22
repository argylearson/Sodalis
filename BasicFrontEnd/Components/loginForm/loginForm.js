class LoginForm extends HTMLFormElement {
    constructor() {
        super();
    }

    connectedCallback() {
        let css = document.createElement("link");
        css.rel = "stylesheet";
        css.type = "text/css";
        css.href = "Components/loginForm/loginForm.css";
        document.head.appendChild(css);

        this.onsubmit = (event) => {
            event.preventDefault();
            this.loginFetch();
        };
        this.innerHTML = "" +
            "<div class='login-form' id='login-form'>" +
                "<div class='email-box'>" +
                    "<label for='emailAddress'><img src='Assets/Icons/email.png' height='100%'></label>" +
                    "<input type='email' id='emailAddress' name='emailAddress' required placeholder='email address'>" +
                "</div>" +
                "<div class='password-box'>" +
                    "<label for='password'><img src='Assets/Icons/password.png' height='100%'></label>" +
                    "<input type='password' id='password' name='password' required placeholder='password'>" +
                "</div>" +
                "<div class='login-button-box'>" +
                    "<button id='login-button' type='submit'><b>Login</b></button>" +
                "</div>" +
            "</div>";
    }

    doLogin() {
        let responsePromise = this.loginFetch();
        responsePromise.then((response) => {
            if (response.ok) {
                response.json().then(body => {
                    window.localStorage.setItem('sodalisToken', body.token);
                    let url = window.location.href.substring(0, -6);
                    window.location.href = url;
                });
            } else {
                if (document.getElementsByClassName('bad-login-alert').length === 0) {
                    let alertElement = document.createElement('div');
                    alertElement.className = 'bad-login-alert';
                    alertElement.innerHTML = "Invalid credentials. Please try again.";
                }
            }
        });
    }

    loginFetch() {
        let url = "https://localhost:5001/api/authentication/login";
        let emailNode = document.getElementById('emailAddress');
        let passwordNode = document.getElementById('password');

        let payload = {
            'emailAddress':emailNode.value,
            'password':passwordNode.value
        };

        fetch(url, {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            headers: {
                'Content-Type': 'application/json'
            },
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(payload)
        }).then((response) => response.json())
            .then((data) => console.log('success: ', data))
            .catch((error) => console.log('error: ', error));
    }
}

console.log("LoginForm added")
customElements.define("login-form", LoginForm, {extends: "form"});
