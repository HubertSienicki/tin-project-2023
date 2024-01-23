import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";
const Home = () => {
	//react variables
	const navigate = useNavigate();

	// State for user credentials and orders
	const [successMessage, setSuccessMessage] = useState("");
	const [errorMessage, setErrorMessage] = useState("");
	const [username, setUsername] = useState("");
	const [products, setProducts] = useState([]);
	const [clients, setClients] = useState([]);
	const [userRole, setUserRole] = useState("");

	// new object states
	const [order, setOrder] = useState({
		productId: "",
		clientId: "",
		quantity: "",
		comments: "",
	});

	const [newClient, setNewClient] = useState({
		firstName: "",
		lastName: "",
		phoneNumber: "",
		email: "",
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

	const logout = () => {
		// Clear the session storage of JWT token
		sessionStorage.removeItem("token");

		// Redirect the user to the login page or home page
		navigate("/login");
	};

	useEffect(() => {
		// decode jwt for a role
		const token = sessionStorage.getItem("token");
		if (token) {
			const payload = decodeToken(token);
			const roleKey =
				"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			const role = payload[roleKey];
			setUserRole(role);

			if (role !== "Admin" && role !== "User") {
				navigate("/unauthorized"); // redirect to unauthorized access page
			}
		} else {
			navigate("/unauthorized"); // redirect to login if no token
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

	const handleNewClientChange = (e) => {
		setNewClient({ ...newClient, [e.target.name]: e.target.value });
	};

	const submitNewClient = async (e) => {
		e.preventDefault();
		// Add logic to post new client data to your server
		const token = sessionStorage.getItem("token");

		try {
			const response = await axios.post(
				"http://localhost:5005/add",
				newClient,
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);
			console.log(response.data);
			// Handle success (clear form, show message, etc.)
		} catch (error) {
			console.error(error);
			// Handle error
		}
	};

	const handleOrderChange = (e) => {
		setOrder({ ...order, [e.target.name]: e.target.value });
	};

	const submitOrder = async (e) => {
		e.preventDefault();
		setSuccessMessage(""); // Reset success message
		setErrorMessage(""); // Reset error message

		const token = sessionStorage.getItem("token");

		try {
			// First API Call: Create Order
			const orderCreateResponse = await axios.post(
				`http://localhost:5003/Order/create?clientId=${order.clientId}`,
				{},
				{ headers: { Authorization: `Bearer ${token}` } }
			);

			const orderId = orderCreateResponse.data.orderId;
			if (!orderId || orderId === 0) {
				setErrorMessage(`Invalid orderId received: ${orderId}`);
				return;
			}

			// Validations for product ID and quantity
			const parsedProductId = parseInt(order.productId, 10);
			const parsedQuantity = parseInt(order.quantity, 10);

			if (isNaN(parsedProductId) || parsedProductId <= 0) {
				setErrorMessage("Invalid or missing Product ID");
				return;
			}

			if (isNaN(parsedQuantity) || parsedQuantity <= 0) {
				setErrorMessage("Invalid or missing Quantity");
				return;
			}

			// Second API Call: Submit Order Details
			try {
				const orderDetailsResponse = await axios.post(
					"http://localhost:5004/Order/Details/create",
					{
						orderId: orderId,
						productId: parsedProductId,
						quantity: parsedQuantity,
						additionalColumn: order.comments,
					},
					{ headers: { Authorization: `Bearer ${token}` } }
				);

				console.log("Order Details Response:", orderDetailsResponse.data);
				setSuccessMessage("Order submitted successfully!");
			} catch (error) {
				console.error("Error submitting order details:", error);
				setErrorMessage("Error submitting order details. Please try again.");
			}
		} catch (error) {
			console.error("Error creating order:", error);
			setErrorMessage("Error creating order. Please try again.");
		}
	};

	return (
		<div className="container">
			{(userRole === "Admin" || userRole === "User") && (
				<div className="form-box">
					<div className="dashboard-container">
						<h1 className="form-title">Home</h1>
						<div className="data-table">
							<h3>Zalogowano</h3>
							<p>Rola użytkownika: {username}</p>
						</div>
						<div className="record-form">
							<h3>Add New Client</h3>
							<form onSubmit={submitNewClient}>
								<input
									type="text"
									name="firstName"
									placeholder="First Name"
									value={newClient.firstName}
									onChange={handleNewClientChange}
								/>
								<input
									type="text"
									name="lastName"
									placeholder="Last Name"
									value={newClient.lastName}
									onChange={handleNewClientChange}
								/>
								<input
									type="text"
									name="phone" // Make sure this matches the state key
									placeholder="Phone Number"
									value={newClient.phone} // Ensure this is bound to the state
									onChange={handleNewClientChange}
								/>
								<input
									type="email"
									name="email"
									placeholder="Email Address"
									value={newClient.email}
									onChange={handleNewClientChange}
								/>
								<button type="submit" className="button mt-20">
									Add Client
								</button>
							</form>
						</div>
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
								{successMessage && (
									<div className="success-message">{successMessage}</div>
								)}
								{errorMessage && (
									<div className="error-message">{errorMessage}</div>
								)}
							</form>
						</div>
						<button onClick={logout} className="button mt-20">
							Logout
						</button>
					</div>
				</div>
			)}
		</div>
	);
};

export default Home;
