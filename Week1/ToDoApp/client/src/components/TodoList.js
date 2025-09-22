import React, { useState, useEffect } from "react";
import axios from "axios";
import TodoForm from "./TodoForm";

function TodoList({ token }) {
  const [todos, setTodos] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const response = await axios.get("http://localhost:5000/api/todo", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setTodos(response.data);
      setError("");
    } catch (err) {
      setError("Failed to fetch todos");
    }
  };

  const handleAddTodo = async (todo) => {
    try {
      const response = await axios.post(
        "http://localhost:5000/api/todo",
        todo,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setTodos([...todos, response.data]);
      setError("");
    } catch (err) {
      setError("Failed to add todo");
    }
  };

  const handleUpdateTodo = async (id, updatedTodo) => {
    try {
      await axios.put(`http://localhost:5000/api/todo/${id}`, updatedTodo, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setTodos(
        todos.map((todo) =>
          todo.id === id ? { ...todo, ...updatedTodo } : todo
        )
      );
      setError("");
    } catch (err) {
      setError("Failed to update todo");
    }
  };

  const handleDeleteTodo = async (id) => {
    try {
      await axios.delete(`http://localhost:5000/api/todo/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setTodos(todos.filter((todo) => todo.id !== id));
      setError("");
    } catch (err) {
      setError("Failed to delete todo");
    }
  };

  return (
    <div>
      <h2>My Todos</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      <TodoForm onAddTodo={handleAddTodo} />
      <ul className="list-group">
        {todos.map((todo) => (
          <li
            key={todo.id}
            className={`list-group-item todo-item ${
              todo.isCompleted ? "completed" : ""
            }`}
          >
            <div>
              <h5>{todo.title}</h5>
              <p>{todo.description}</p>
              <input
                type="checkbox"
                checked={todo.isCompleted}
                onChange={(e) =>
                  handleUpdateTodo(todo.id, {
                    ...todo,
                    isCompleted: e.target.checked,
                  })
                }
              />
            </div>
            <button
              className="btn btn-danger btn-sm"
              onClick={() => handleDeleteTodo(todo.id)}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default TodoList;
