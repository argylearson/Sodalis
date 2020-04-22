class GoalForm extends HTMLFormElement {
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
            "<div class='goal-form' id='goal-form'>" +
                "<div class='title-box'>" +
                    "<input type='text' id='title' name='title' required placeholder='Title'>" +
                "</div>" +
                "<div class='description-box'>" +
                    "<textarea id='description' name='description' required placeholder='Description'></textarea>" +
                "</div>" +
                "<div id='bottom-row'>" +
                    "<div class='status-box'>" +
                    "<label for='status'>Status</label>" +
                        "<select id='status' name='status'></select>" +
                    "</div>" +
                    "<div class='public-box'>" +
                        "<label for='isPublic'>Public</label>" +
                        "<input type='checkbox' id='isPublic' name='isPublic'>" +
                    "</div>" +
                    "<div class='goal-button-box'>" +
                        "<button id='goal-button' type='submit'><b>Submit</b></button>" +
                    "</div>" +
                "</div>" +
            "</div>";
        this.statusFetch();
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

    statusFetch(status) {
        let url = "https://localhost:5001/api/goals/statuses";
        const token = localStorage.getItem('sodalisToken');
        fetch(url, {
            method: 'GET',
            mode: 'cors',
            cache: 'no-cache',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }).then((response) => {
            if (response.ok) {
                response.json().then((statuses) => {
                    let statusSelector = document.getElementById('status');
                    statuses.forEach((statusValue) => {
                        statusSelector.innerHTML += "" +
                            "<option value=''" + statusValue.id +"'>" + statusValue.name + "</option>";
                    });
                });
            }
            else {
                console.error("Error loading goal statuses");
            }
        })
    }

    static initialize() {
        let css = document.createElement("link");
        css.rel = "stylesheet";
        css.type = "text/css";
        css.href = "Components/goalForm/GoalForm.css";
        document.head.appendChild(css);
    }
}

GoalForm.initialize();
console.log("GoalForm added")
customElements.define("goal-form", GoalForm, {extends: "form"});
