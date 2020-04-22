class GoalForm extends HTMLFormElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.action = "index.html";
        this.onsubmit = (event) => {
            event.preventDefault();
            this.postGoalFetch();
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

    postGoalFetch() {
        let url = "https://localhost:5001/api/goals";
        let method = 'POST';

        const urlParams = new URLSearchParams(window.location.search);
        const urlId = parseInt(urlParams.get('goalId'));
        if (urlId) {
            url += "/" + urlId;
            method = 'PATCH';
        }

        let titleNode = document.getElementById('title');
        let descriptionNode = document.getElementById('description');
        let statusNode = document.getElementById('status');
        let publicNode = document.getElementById('isPublic');

        let payload = {
            'title': titleNode.value,
            'description': descriptionNode.value,
            'status': parseInt(statusNode.options[statusNode.selectedIndex].value),
            'isPublic': publicNode.checked
        };

        fetch(url, {
            method: method,
            mode: 'cors',
            cache: 'no-cache',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'bearer ' + localStorage.getItem('sodalisToken')
            },
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(payload)
        }).then((response) => {
            if (response.ok) {
                response.json()
                    .then((data) => {
                        let url = window.location.href;
                        window.location.href = url.substring(0, url.lastIndexOf('/')) + "/index.html";
                    })
                    .catch((error) => console.log('error: ', error));
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
                            "<option value='" + statusValue.id +"'>" + statusValue.name + "</option>";
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
