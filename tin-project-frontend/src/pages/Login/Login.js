import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";

const Login = () => {
	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState("");
	const navigate = useNavigate();

	useEffect(() => {
		const token = sessionStorage.getItem("token");
		if (token) {
			const role = decodeToken(token).role;
			navigateBasedOnRole(role);
		}
	}, [navigate]);

	const navigateBasedOnRole = (role) => {
		if (role === "Admin") {
			navigate("/dashboard");
		} else if (role === "User") {
			navigate("/");
		}
	};

	const decodeToken = (token) => {
		const base64Url = token.split(".")[1];
		const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
		const jsonPayload = decodeURIComponent(
			atob(base64)
				.split("")
				.map((c) => {
					return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
				})
				.join("")
		);

		return JSON.parse(jsonPayload);
	};

	const handleSubmit = async (e) => {
		e.preventDefault();

		try {
			const response = await axios.post("http://localhost:5001/Users/login", {
				username,
				password,
			});

			if (response.status === 200) {
				// Save token to session storage
				sessionStorage.setItem("token", response.data.token); 
				// In your handleSubmit function, after saving the token:
				const payload = decodeToken(response.data.token);


				// Access the role using the full key
				const roleKey =
					"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
				const role = payload[roleKey];
				navigateBasedOnRole(role);
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
