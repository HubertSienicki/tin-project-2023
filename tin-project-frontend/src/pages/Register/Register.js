import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

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
			<form onSubmit={handleSubmit}>
				<h2>Rejestracja</h2>
				<div className="mb-3">
					<label htmlFor="username" className="form-label">
						Nazwa użytkownika
					</label>
					<input
						type="text"
						className="form-control"
						id="username"
						value={username}
						onChange={(e) => setUsername(e.target.value)}
						required
					/>
				</div>
				<div className="mb-3">
					<label htmlFor="email" className="form-label">
						Email
					</label>
					<input
						type="email"
						className="form-control"
						id="email"
						value={email}
						onChange={(e) => setEmail(e.target.value)}
						required
					/>
				</div>
				<div className="mb-3">
					<label htmlFor="password" className="form-label">
						Hasło
					</label>
					<input
						type="password"
						className="form-control"
						id="password"
						value={password}
						onChange={(e) => setPassword(e.target.value)}
						required
					/>
				</div>
				<div className="mb-3">
					<label htmlFor="confirmPassword" className="form-label">
						Potwierdź hasło
					</label>
					<input
						type="password"
						className="form-control"
						id="confirmPassword"
						value={confirmPassword}
						onChange={(e) => setConfirmPassword(e.target.value)}
						required
					/>
				</div>
				<button type="submit" className="btn btn-primary">
					Zarejestruj się
				</button>
			</form>
		</div>
	);
};

export default Register;
