class GoalContainer extends HTMLDivElement {

    constructor() {
        super();
    }

    connectedCallback() {
        this.className = "sodalis-goal-container";

        const token = localStorage.getItem('sodalisToken');
        if (token) {
            let url = "https://localhost:5001/api/goals";
            fetch(url, {
                method: 'GET',
                mode: 'cors',
                cache: 'no-cache',
                headers: {
                    'Authorization': 'Bearer ' + token
                }
            }).then((response) => {
                if (response.ok) {
                    response.json().then((goals) => {
                        goals.forEach((goal) => {
                            let goalElement = new Goal();
                            Goal.JsonToDom(goal, goalElement);
                            this.appendChild(goalElement);
                        });
                    })
                } else {
                    console.error("Error loading goal " + this.goalId);
                }
            })
        } else {
            window.location.replace("login.html");
        }
    }
    static initialize()
    {
        let goalJs = document.createElement("script");
        goalJs.src = "Components/goal/goal.js";
        document.head.appendChild(goalJs);

        let goalCss = document.createElement("link");
        goalCss.rel = "stylesheet";
        goalCss.type = "text/css";
        goalCss.href = "Components/goalContainer/goalContainer.css";
        document.head.appendChild(goalCss);

        let css = document.createElement("link");
        css.rel = "stylesheet";
        css.type = "text/css";
        css.href = "Components/goal/goal.css";
        document.head.appendChild(css);
    }
}

GoalContainer.initialize();
console.log("Goals added")
customElements.define("sodalis-goal-container", GoalContainer, {extends: "div"});
