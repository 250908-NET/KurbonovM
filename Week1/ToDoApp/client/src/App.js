import React, { useState } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Login from "./components/Login";
import Register from "./components/Register";
import TodoList from "./components/TodoList";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token") || "");

  const handleLogin = (newToken) => {
    setToken(newToken);
    localStorage.setItem("token", newToken);
  };

  const handleLogout = () => {
    setToken("");
    localStorage.removeItem("token");
  };

  return (
    <Router>
      <div className="container">
        <h1 className="my-4">Todo App</h1>
        {token && (
          <button className="btn btn-danger mb-3" onClick={handleLogout}>
            Logout
          </button>
        )}
        <Routes>
          <Route
            path="/login"
            element={
              token ? <Navigate to="/todos" /> : <Login onLogin={handleLogin} />
            }
          />
          <Route
            path="/register"
            element={token ? <Navigate to="/todos" /> : <Register />}
          />
          <Route
            path="/todos"
            element={
              token ? <TodoList token={token} /> : <Navigate to="/login" />
            }
          />
          <Route
            path="/"
            element={<Navigate to={token ? "/todos" : "/login"} />}
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
