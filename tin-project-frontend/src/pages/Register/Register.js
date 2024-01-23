import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";

const Register = () => {
	const [username, setUsername] = useState("");
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [confirmPassword, setConfirmPassword] = useState("");
	const [error, setError] = useState("");
	const navigate = useNavigate();

	const handleSubmit = async (e) => {
		e.preventDefault();

		if (password !== confirmPassword) {
			setError("Hasła nie są takie same. Spróbuj ponownie.");
			return;
		}

		try {
			const response = await axios.post(
				"http://localhost:5001/Users/register",
				{
					username,
					email,
					password,
				}
			);

			if (response.status === 200 || response.status === 201) {
				console.log("Registration successful:", response.data);
				// Redirect to login or another page
				navigate("/login");
			} else {
				// Handle other successful responses if necessary
			}
		} catch (error) {
			console.error("Registration error:", error);
			setError(
				error.response.data.message ||
					"An error occurred during registration. Please try again later."
			);
		}
	};

	return (
		<div className="container">
			<div className="form-box">
				<h2 className="form-title">Rejestracja</h2>
				<form onSubmit={handleSubmit}>
					<div className="form-control">
						<label htmlFor="username">Nazwa użytkownika</label>
						<input
							type="text"
							id="username"
							value={username}
							onChange={(e) => setUsername(e.target.value)}
						/>
					</div>
					<div className="form-control">
						<label htmlFor="email">Email</label>
						<input
							type="email"
							id="email"
							value={email}
							onChange={(e) => setEmail(e.target.value)}
						/>
					</div>
					<div className="form-control">
						<label htmlFor="password">Hasło</label>
						<input
							type="password"
							id="password"
							value={password}
							onChange={(e) => setPassword(e.target.value)}
						/>
					</div>
					<div className="form-control">
						<label htmlFor="confirmPassword">Potwierdź hasło</label>
						<input
							type="password"
							id="confirmPassword"
							value={confirmPassword}
							onChange={(e) => setConfirmPassword(e.target.value)}
						/>
					</div>
					{error && <div className="error-message">{error}</div>}
					<button type="submit" className="btn-primary">
						Zarejestruj się
					</button>
					<a href="/login" className="link">
						Masz już konto? Zaloguj się
					</a>
				</form>
			</div>
		</div>
	);
};

export default Register;
