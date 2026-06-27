



const API_BASE = "https://localhost:7298/api";


// Load users when page loads
async function loadUsers() {
    try {
        const response = await fetch(`${API_BASE}/users`);
        const result = await response.json();

        const userSelect = document.getElementById("User");
        userSelect.innerHTML = "";

        if (result.success && result.data.length > 0) {

            userSelect.innerHTML = '<option value="">Select User</option>';

            result.data.forEach(user => {
                const option = document.createElement("option");
                option.value = user.userId;
                option.textContent = user.userName;
                userSelect.appendChild(option);
            });

        } else {
            userSelect.innerHTML =
                '<option value="">No users found</option>';
        }

    } catch (error) {
        console.error(error);
        alert("Failed to load users.");
    }
}

loadUsers();

/ Submit form
document.getElementById("taskForm").addEventListener("submit", async function (e) {

    e.preventDefault();

    const task = {
        title: document.getElementById("title").value,
        userId: parseInt(document.getElementById("User").value),
        status: document.getElementById("status").value
    };

    try {

        const response = await fetch(`${API_BASE}/tasks`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Task created successfully!");

            document.getElementById("taskForm").reset();
        }
        else {
            alert(result.message);
        }

    } catch (error) {
        console.error(error);
        alert("Error creating task.");
    }

});

document.addEventListener("DOMContentLoaded", loadDashboard);

async function loadDashboard() {
    try {
        const response = await fetch(API_URL);

        if (!response.ok) {
            throw new Error("Failed to fetch tasks");
        }

        const result = await response.json();


        const tasks = result.data || result.Data || [];

        updateDashboard(tasks);

    } catch (error) {
        console.error(error);
        alert("Unable to load dashboard");
    }
}

function updateDashboard(tasks) {

    document.getElementById("totalCount").textContent = tasks.length;

    const todo = tasks.filter(t => t.status === "Todo").length;

    const inProgress = tasks.filter(t => t.status === "In Progress").length;

    const done = tasks.filter(t => t.status === "Done").length;

    document.getElementById("todoCount").textContent = todo;
    document.getElementById("inProgressCount").textContent = inProgress;
    document.getElementById("doneCount").textContent = done;
}



const taskForm = document.getElementById("taskForm");
const title = document.getElementById("title");
const status = document.getElementById("status");
const user = document.getElementById("user");

// Get task id from URL
const params = new URLSearchParams(window.location.search);
const taskId = params.get("id");

// Load users
async function loadUsers() {
    try {
        const response = await fetch(`${API_URL}/Users`);
        const result = await response.json();

        user.innerHTML = "";

        result.data.forEach(u => {
            user.innerHTML += `
                <option value="${u.id}">
                    ${u.name}
                </option>`;
        });

    } catch (err) {
        console.log(err);
    }
}

// Load task details
async function loadTask() {
    try {
        const response = await fetch(`${API_URL}/Tasks/${TaskId}`);
        const result = await response.json();

        const task = result.data;

        Title.value = task.Title;
        Status.value = task.Status;
        User.value = task.UserId;

    } catch (err) {
        console.log(err);
    }
}

// Update task
taskForm.addEventListener("submit", async function (e) {

    e.preventDefault();

    const task = {
        title: title.value,
        userId: parseInt(user.value),
        status: status.value
    };

    try {

        const response = await fetch(`${API_URL}/Tasks/${taskId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Task updated successfully.");
            window.location.href = "tasks.html";
        }
        else {
            alert(result.message);
        }

    } catch (err) {
        console.log(err);
    }

});

async function init() {
    await loadUsers();
    await loadTask();
}

init();


const taskForm = document.getElementById("taskForm");
const title = document.getElementById("title");
const status = document.getElementById("status");
const user = document.getElementById("user");

// Get task id from URL
const params = new URLSearchParams(window.location.search);
const taskId = params.get("id");

// Load users
async function loadUsers() {
    try {
        const response = await fetch(`${API_URL}/Users`);
        const result = await response.json();

        user.innerHTML = "";

        result.data.forEach(u => {
            user.innerHTML += `
                <option value="${u.id}">
                    ${u.name}
                </option>`;
        });

    } catch (err) {
        console.log(err);
    }
}

// Load task details
async function loadTask() {
    try {
        const response = await fetch(`${API_URL}/Tasks/${TaskId}`);
        const result = await response.json();

        const task = result.data;

        Title.value = task.Title;
        Status.value = task.Status;
        User.value = task.UserId;

    } catch (err) {
        console.log(err);
    }
}

// Update task
taskForm.addEventListener("submit", async function (e) {

    e.preventDefault();

    const task = {
        title: title.value,
        userId: parseInt(user.value),
        status: status.value
    };

    try {

        const response = await fetch(`${API_URL}/Tasks/${taskId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Task updated successfully.");
            window.location.href = "tasks.html";
        }
        else {
            alert(result.message);
        }

    } catch (err) {
        console.log(err);
    }

});

async function init() {
    await loadUsers();
    await loadTask();
}

init();


