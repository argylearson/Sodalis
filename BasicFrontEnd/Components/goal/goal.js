class Goal extends HTMLDivElement {

    constructor() {
        super();
    }

    static get observedAttributes() {
        return ['goalId'];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        console.log(name + " is now " + newValue);
    }

    get goalId() {
        if (this.hasAttribute('goalId'))
            return parseInt(this.getAttribute('goalId'));
        return -1;
    }
    set goalId(value) {
        this.setAttribute('goalId', value);
    }

    connectedCallback() {
        this.className = "sodalis-goal";

        if (this.goalId > 0) {
            const token = localStorage.getItem('sodalisToken');
            if (token) {
                let url = "https://localhost:5001/api/goals/" + this.goalId;
                fetch(url, {
                    method: 'GET',
                    mode: 'cors',
                    cache: 'no-cache',
                    headers: {
                        'Authorization': 'Bearer ' + token
                    }
                }).then((response) => {
                    if (response.ok) {
                        response.json().then((goal) =>
                            Goal.JsonToDom(goal, this)
                        );
                    }
                    else {
                        console.error("Error loading goal " + this.goalId);
                    }
                })
            }
            else {
                window.location.replace("login.html");
            }
        }
    }

    updateForStatus(status) {
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
                    statuses.forEach((statusValue) => {
                        if (status === statusValue.id) {
                            this.innerHTML += "" +
                                "<p><b>" + statusValue.name + "</b></p>";
                        }
                    });
                });
            }
            else {
                console.error("Error loading goal statuses");
            }
        })
    }

    static JsonToDom(jsonObject, goal) {
        goal.innerHTML = "" +
            "<p>UserId: " + jsonObject.userId + "</p>" +
            "<p><b>" + jsonObject.title + "</b></p>" +
            "<p>" + jsonObject.description + "</p>";
        goal.updateForStatus(jsonObject.status);
    }
}

let css = document.createElement("link");
css.rel = "stylesheet";
css.type = "text/css";
css.href = "Components/goal/goal.css";
document.head.appendChild(css);

console.log("Goals added")
customElements.define("sodalis-goal", Goal, {extends: "div"});
