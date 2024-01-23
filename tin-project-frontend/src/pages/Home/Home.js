import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";
const Home = () => {
	// State for user credentials and orders
	const [username, setUsername] = useState("");
	const [products, setProducts] = useState([]);
	const [clients, setClients] = useState([]);
	const [order, setOrder] = useState({
		productId: "",
		clientId: "",
		quantity: "",
		comments: "",
	});

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

	useEffect(() => {
		// decode jwt for a role
		const token = sessionStorage.getItem("token");
		if (token) {
			const payload = decodeToken(token);
			const roleKey =
				"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			const role = payload[roleKey];
			console.log(role);
			setUsername(role);
		}

		// Fetch products
		const fetchProducts = async () => {
			try {
				const response = await axios.get("http://localhost:5002/Product", {
					headers: {
						Authorization: `Bearer ${token}`,
					},
				});
				setProducts(response.data);
				console.log(response.data);
			} catch (error) {
				console.error(error);
			}
		};

		fetchProducts();

		// Fetch clients
		const fetchClients = async () => {
			try {
				const response = await axios.get("http://localhost:5005/Client", {
					headers: {
						Authorization: `Bearer ${token}`,
					},
				});
				setClients(response.data);
				console.log(response.data);
			} catch (error) {
				console.error(error);
			}
		};
		fetchClients();
	}, []);

	const handleOrderChange = (e) => {
		setOrder({ ...order, [e.target.name]: e.target.value });
	};

	const submitOrder = async (e) => {
		e.preventDefault();

		console.log("Order Data:", order);

		const token = sessionStorage.getItem("token");

		try {
			const orderCreateResponse = await axios.post(
				`http://localhost:5003/Order/create?clientId=${order.clientId}`,
				{},
				{
					headers: { Authorization: `Bearer ${token}` },
				}
			);

			const orderId = orderCreateResponse.data.orderId;
			if (!orderId || orderId === 0) {
				console.error("Invalid orderId received:", orderId);
				return;
			}

			console.log("Product ID:", order.productId);
			console.log("Quantity:", order.quantity);

			const parsedProductId = parseInt(order.productId, 10);
			const parsedQuantity = parseInt(order.quantity, 10);

			// Check if parsing resulted in valid numbers
			if (isNaN(parsedProductId) || parsedProductId <= 0) {
				console.error("Invalid or missing Product ID");
				return;
			}

			if (isNaN(parsedQuantity) || parsedQuantity <= 0) {
				console.error("Invalid or missing Quantity");
				return;
			}

			try {
				const orderDetailsResponse = await axios.post(
					"http://localhost:5004/Order/Details/create",
					{
						orderId: orderId,
						productId: parsedProductId,
						quantity: parsedQuantity,
						additionalColumn: order.comments,
					},
					{
						headers: { Authorization: `Bearer ${token}` },
					}
				);

				console.log("Order Details Response:", orderDetailsResponse.data);
				console.log("Order submitted successfully");
			} catch (error) {
				console.error("Error submitting order details:", error);
			}
		} catch (error) {
			console.error(
				"Error submitting order:",
				error.response ? error.response.data : error
			);
		}
	};
	return (
		<div className="container">
			<div className="form-box">
				<div className="dashboard-container">
					<h1 className="form-title">Home</h1>
					{
						<div className="data-table">
							<h3>Zalogowano</h3>
							<p>Rola użytkownika: {username}</p>
						</div>
					}

					<div className="record-form">
						<h3>Dodaj zamówienie</h3>
						<form onSubmit={submitOrder}>
							<select
								name="productId"
								onChange={handleOrderChange}
								value={order.productId}
							>
								<option value="">Wybierz produkt</option>
								{products.map((product) => (
									<option key={product.id} value={product.id}>
										{product.name}
									</option>
								))}
							</select>

							<select
								name="clientId"
								onChange={handleOrderChange}
								value={order.clientId}
							>
								<option value="">Wybierz klienta</option>
								{clients.map((client) => (
									<option key={client.id} value={client.id}>
										{client.firstName} {client.lastName}
									</option>
								))}
							</select>

							<input
								type="number"
								name="quantity"
								placeholder="Ilość"
								value={order.quantity}
								onChange={handleOrderChange}
							/>

							<textarea
								name="comments"
								placeholder="Dodatkowe komentarze"
								value={order.comments}
								onChange={handleOrderChange}
							></textarea>

							<button type="submit" className="button mt-20">
								Add Order
							</button>
						</form>
					</div>
				</div>
			</div>
		</div>
	);
};

export default Home;
