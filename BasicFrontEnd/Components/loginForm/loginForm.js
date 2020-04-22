class LoginForm extends HTMLFormElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.action = "index.html";
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
        }).then((response) => {
            if (response.ok) {
                response.json()
                    .then((data) => {
                        window.localStorage.setItem('sodalisToken', data.token);
                        let url = window.location.href;
                        window.location.href = url.substring(0, url.lastIndexOf('/')) + "/index.html";
                    })
                    .catch((error) => console.log('error: ', error));
            } else {
                if (document.getElementsByClassName('bad-login-alert').length === 0) {
                    let alertElement = document.createElement('div');
                    alertElement.className = 'bad-login-alert';
                    alertElement.innerHTML = "Invalid credentials. Please try again.";
                }
            }
        })
    }
    static initialize() {
        let css = document.createElement("link");
        css.rel = "stylesheet";
        css.type = "text/css";
        css.href = "Components/loginForm/loginForm.css";
        document.head.appendChild(css);
    }
}

LoginForm.initialize();
console.log("LoginForm added")
customElements.define("login-form", LoginForm, {extends: "form"});
