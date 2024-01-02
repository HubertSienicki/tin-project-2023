import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/login-style.css";

const Register = () => {
	const [username, setUsername] = useState("");
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [confirmPassword, setConfirmPassword] = useState("");
	const navigate = useNavigate();

	const handleSubmit = async (e) => {
		e.preventDefault();
		if (password !== confirmPassword) {
			alert("Hasła nie są takie same!");
			return;
		}
		//  try {
		//    // Wywołaj API do rejestracji
		//    await axios.post('API_ENDPOINT/register', { username, email, password });
		//    navigate('/login'); // Przekierowanie do logowania po pomyślnej rejestracji
		//  } catch (error) {
		//    // Obsługa błędów
		//  }
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
