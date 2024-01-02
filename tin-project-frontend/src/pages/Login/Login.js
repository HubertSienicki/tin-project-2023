import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/login-style.css";

const Login = () => {
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const navigate = useNavigate();

	const handleSubmit = (e) => {
		// e.preventDefault();
		console.log("Submitted");
	};

	return (
		<div className="container">
			<div className="form-box">
				<h2 className="form-title">Logowanie</h2>
				<form>
					<div className="form-control">
						<label htmlFor="email">Email</label>
						<input
							type="email"
							onChange={(e) => setEmail(e.target.value)}
							id="email"
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
					<button type="submit" onSubmit={handleSubmit} className="btn-primary">
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
