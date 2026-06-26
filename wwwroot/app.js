// Base API URL
const API_BASE = "https://localhost:7298/api";

// Utility: Fetch JSON
async function fetchJSON(url, options = {}) {
    const response = await fetch(url, options);
    if (!response.ok) throw new Error(`Error: ${response.status}`);
    return response.json();
}

// ---------------- TASKS ----------------

// Load all tasks into table
async function loadTasks() {
    const tasks = await fetchJSON(`${API_BASE}/Tasks`);
    const tbody = document.querySelector("#taskTable tbody");
    if (!tbody) return;
    tbody.innerHTML = "";
    tasks.forEach(task => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${task.title}</td>
            <td>${task.userName || "Unassigned"}</td>
            <td>${task.status}</td>
            <td>
                <button onclick="editTask(${task.id})">Edit</button>
                <button onclick="deleteTask(${task.id})">Delete</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Create new task
async function createTask(e) {
    e.preventDefault();
    const title = document.getElementById("title").value;
    const userId = document.getElementById("user").value;
    const status = document.getElementById("status").value;

    await fetchJSON(`${API_BASE}/Tasks`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, userId, status })
    });
    alert("Task created!");
    window.location.href = "tasks.html";
}

// Update existing task
async function updateTask(e, taskId) {
    e.preventDefault();
    const title = document.getElementById("title").value;
    const userId = document.getElementById("user").value;
    const status = document.getElementById("status").value;

    await fetchJSON(`${API_BASE}/Tasks/${taskId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id: taskId, title, userId, status })
    });
    alert("Task updated!");
    window.location.href = "tasks.html";
}

// Delete task
async function deleteTask(id) {
    await fetchJSON(`${API_BASE}/Tasks/${id}`, { method: "DELETE" });
    alert("Task deleted!");
    loadTasks();
}

// ---------------- USERS ----------------

// Load users into dropdown
async function loadUsersDropdown() {
    const users = await fetchJSON(`${API_BASE}/Users`);
    const select = document.getElementById("user");
    if (!select) return;
    select.innerHTML = `<option value="">Select user</option>`;
    users.forEach(u => {
        const opt = document.createElement("option");
        opt.value = u.id;
        opt.textContent = u.name;
        select.appendChild(opt);
    });
}

// Load users into table
async function loadUsersTable() {
    const users = await fetchJSON(`${API_BASE}/Users`);
    const tbody = document.get("#userTable tbody");
    if (!tbody) return;
    tbody.innerHTML = "";
    users.forEach(user => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td><button onclick="viewUser(${user.id})">View</button></td>
        `;
        tbody.appendChild(row);
    });
}

// Add new user
async function addUser(e) {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;

    await fetchJSON(`${API_BASE}/Users`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, email })
    });
    alert("User added!");
    loadUsersTable();
}

// ---------------- DASHBOARD ----------------

// Load dashboard counts
async function loadDashboard() {
    const tasks = await fetchJSON(`${API_BASE}/Tasks`);
    document.getElementById("totalCount").textContent = tasks.length;
    document.getElementById("todoCount").textContent = tasks.filter(t => t.status === "Todo").length;
    document.getElementById("inProgressCount").textContent = tasks.filter(t => t.status === "In Progress").length;
    document.getElementById("doneCount").textContent = tasks.filter(t => t.status === "Done").length;
}

// ---------------- INIT ----------------

// Attach event listeners depending on page
document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById("taskForm")) {
        const form = document.getElementById("taskForm");
        if (form.querySelector("button").textContent.includes("Create")) {
            form.addEventListener("submit", createTask);
        } else {
            // Example: pass taskId from query string
            const taskId = new URLSearchParams(window.location.search).get("id");
            form.addEventListener("submit", (e) => updateTask(e, taskId));
        }
        loadUsersDropdown();
    }

    if (document.getElementById("taskTable")) loadTasks();
    if (document.getElementById("userTable")) loadUsersTable();
    if (document.getElementById("userForm")) document.getElementById("userForm").addEventListener("submit", addUser);
    if (document.getElementById("totalCount")) loadDashboard();
});
