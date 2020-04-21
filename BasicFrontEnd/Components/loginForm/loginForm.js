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


        //this.className = "login-form";
        this.action = "https://localhost:5001/api/authentication/login";
        this.method = "post";
        this.innerHTML = "" +
            "<div class='login-form'>" +
                "<h3 class='login-text'>Login</h3>" +
                "<div class='email-box' style='height:30px'>" +
                    "<label for='emailAddress' style='width:30px'><img src='Assets/Icons/email.png' height='100%'></label>" +
                    "<input type='email' id='emailAddress' name='emailAddress' required='required'>" +
                "</div>" +
                "<div class='password-box' style='height:30px'>" +
                    "<label for='password' style='width:30px'><img src='Assets/Icons/password.png' height='100%'></label>" +
                    "<input type='password' id='password' name='password'" +
                "</div>" +
            "</div>";
    }
}

console.log("LoginForm added")
customElements.define("login-form", LoginForm, {extends: "form"});
