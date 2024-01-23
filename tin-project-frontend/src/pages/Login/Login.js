import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";

const Login = () => {
	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState("");
	const navigate = useNavigate();

	const handleSubmit = async (e) => {
		e.preventDefault();

		try {
			const response = await axios.post("http://localhost:5001/Users/login", {
				username,
				password,
			});

			if (response.status === 200) {
				console.log("Login successful:", response.data);
				navigate("/dashboard");
			}
		} catch (error) {
			console.error("Login error:", error);
			setError("Nieprawidłowe dane logowania. Spróbuj ponownie.");
		}
	};

	return (
		<div className="container">
			<div className="form-box">
				<h2 className="form-title">Logowanie</h2>
				<form onSubmit={handleSubmit}>
					<div className="form-control">
						<label htmlFor="Username">Nazwa Użytkownika</label>
						<input
							type="username"
							onChange={(e) => setUsername(e.target.value)}
							id="username"
						/>
					</div>
					<div className="form-control">
						<label htmlFor="password">Hasło</label>
						<input
							type="password"
							onChange={(e) => setPassword(e.target.value)}
							id="password"
						/>
					</div>

					{error && <div className="error-message">{error}</div>}
					<button type="submit" className="btn-primary">
						Zaloguj się
					</button>
					<a href="/register" className="link">
						Nie masz konta? Zarejestruj się
					</a>
				</form>
			</div>
		</div>
	);
};

export default Login;
