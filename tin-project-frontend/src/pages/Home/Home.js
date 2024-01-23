import axios from "axios";
import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { fetchClientsApi } from "../../api/ClientApi";
import { fetchProductsApi } from "../../api/ProductApi";
import "../../assets/styles/styles.css";
import { decodeToken } from "../../services/JWTService";
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

	// error states

	const [clientSuccessMessage, setClientSuccessMessage] = useState("");
	const [clientErrorMessage, setClientErrorMessage] = useState("");

	const [productSuccessMessage, setProductSuccessMessage] = useState("");
	const [productErrorMessage, setProductErrorMessage] = useState("");

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
		phone: "",
		email: "",
	});

	const [newProduct, setNewProduct] = useState({
		productName: "",
		productPrice: "",
	});

	const [clientErrors, setClientErrors] = useState({
		firstNameError: "",
		lastNameError: "",
		phoneNumberError: "",
		emailError: "",
	});

	const [productErrors, setProductErrors] = useState({
		productNameError: "",
		productPriceError: "",
	});

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
		const fetchProductsData = async () => {
			const productsData = await fetchProductsApi(token);
			setProducts(productsData);
		};

		// Fetch clients
		const fetchClientData = async () => {
			const clientsData = await fetchClientsApi(token);
			setClients(clientsData);
		};

		fetchProductsData();
		fetchClientData();
	}, []);

	const handleNewClientChange = (e) => {
		setNewClient({ ...newClient, [e.target.name]: e.target.value });
	};

	const submitNewClient = async (e) => {
		e.preventDefault();
		// Reset messages
		setClientErrors({
			firstNameError: "",
			lastNameError: "",
			phoneNumberError: "",
			emailError: "",
		});

		// Basic validation with individual error messages
		let errors = {};
		if (!newClient.firstName) errors.firstNameError = "First name is required.";
		if (!newClient.lastName) errors.lastNameError = "Last name is required.";
		const phoneRegex = /^\d+$/;
		if (!newClient.phone) {
			errors.phoneNumberError = "Phone number is required.";
		} else if (!phoneRegex.test(newClient.phone)) {
			errors.phoneNumberError = "Invalid phone number format.";
		}

		if (!newClient.email) {
			errors.emailError = "Email is required.";
		} else {
			// Email validation using regex
			const emailRegex = /\S+@\S+\.\S+/;
			if (!emailRegex.test(newClient.email)) {
				errors.emailError = "Invalid email format.";
			}
		}

		if (Object.keys(errors).length > 0) {
			setClientErrors(errors);
			return;
		}

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
			setClientSuccessMessage("Client added successfully!");
			setNewClient({ firstName: "", lastName: "", phoneNumber: "", email: "" });
		} catch (error) {
			setClientErrorMessage("Failed to add client. Please try again.");
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

	const handleProductChange = (e) => {
		setNewProduct({ ...newProduct, [e.target.name]: e.target.value });
	};

	// Handle product form submission
	const submitProduct = async (e) => {
		e.preventDefault();
		// Reset messages
		setProductErrors({
			productNameError: "",
			productPriceError: "",
		});

		// Basic validation with individual error messages
		let errors = {};
		if (!newProduct.productName)
			errors.productNameError = "Product name is required.";
		if (!newProduct.productPrice) {
			errors.productPriceError = "Product price is required.";
		} else if (isNaN(newProduct.productPrice) || newProduct.productPrice <= 0) {
			errors.productPriceError =
				"Invalid product price. Price must be a positive number.";
		}

		if (Object.keys(errors).length > 0) {
			setProductErrors(errors);
			return;
		}

		const token = sessionStorage.getItem("token");

		try {
			const response = await axios.post(
				"http://localhost:5002/Product",
				{
					name: newProduct.productName,
					price: parseFloat(newProduct.productPrice),
				},
				{
					headers: {
						Authorization: `Bearer ${token}`,
						"Content-Type": "application/json",
					},
				}
			);
			console.log(response.data);
			setProductSuccessMessage("Product added successfully!");
			setNewProduct({ productName: "", productPrice: "" }); // Reset form
		} catch (error) {
			console.error(error);
			setProductErrorMessage("Failed to add product. Please try again.");
		}
	};

	return (
		<div className="container">
			{(userRole === "Admin" || userRole === "User") && (
				<div className="form-box">
					<nav className="navbar">
						<Link to="/dashboard" className="nav-link">
							Dashboard
						</Link>{" "}
						{/* Link to the home page */}
						{/* Add other navigation links if needed */}
					</nav>
					<div className="dashboard-container">
						<div className="dashboard-information">
							<h1 className="form-title">Home</h1>
							<div className="data-table">
								<h3>You are now logged in.</h3>
								<p>Your current role is {userRole}</p>
							</div>
						</div>
						<div className="top-right">
							<button onClick={logout} className="button mt-20">
								Logout
							</button>
						</div>
						<div
							className="forms-container"
							style={{ display: "flex", justifyContent: "space-between" }}
						>
							<div className="record-form">
								<h3>Add New Product</h3>
								<form onSubmit={submitProduct}>
									<input
										type="text"
										name="productName"
										placeholder="Product Name"
										value={newProduct.productName}
										onChange={handleProductChange}
									/>
									{productErrors.productNameError && (
										<div className="error-message">
											{productErrors.productNameError}
										</div>
									)}
									<input
										type="text"
										name="productPrice"
										placeholder="Product Price (per unit)"
										value={newProduct.productPrice}
										onChange={handleProductChange}
									/>
									{productErrors.productPriceError && (
										<div className="error-message">
											{productErrors.productPriceError}
										</div>
									)}
									<button type="submit" className="button mt-20">
										Add Product
									</button>
									{productSuccessMessage && (
										<div className="success-message">
											{productSuccessMessage}
										</div>
									)}
									{productSuccessMessage && (
										<div className="error-message">{productErrorMessage}</div>
									)}
								</form>
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
									{clientErrors.firstNameError && (
										<div className="error-message">
											{clientErrors.firstNameError}
										</div>
									)}
									<input
										type="text"
										name="lastName"
										placeholder="Last Name"
										value={newClient.lastName}
										onChange={handleNewClientChange}
									/>
									{clientErrors.lastNameError && (
										<div className="error-message">
											{clientErrors.lastNameError}
										</div>
									)}
									<input
										type="text"
										name="phone" // Make sure this matches the state key
										placeholder="Phone Number"
										value={newClient.phone} // Ensure this is bound to the state
										onChange={handleNewClientChange}
									/>
									{clientErrors.phoneNumberError && (
										<div className="error-message">
											{clientErrors.phoneNumberError}
										</div>
									)}
									<input
										type="email"
										name="email"
										placeholder="Email Address"
										value={newClient.email}
										onChange={handleNewClientChange}
									/>
									{clientErrors.emailError && (
										<div className="error-message">
											{clientErrors.emailError}
										</div>
									)}
									<button type="submit" className="button mt-20">
										Add Client
									</button>
									{clientSuccessMessage && (
										<div className="success-message">
											{clientSuccessMessage}
										</div>
									)}
									{clientErrorMessage && (
										<div className="error-message">{clientErrorMessage}</div>
									)}
								</form>
							</div>
							<div className="record-form">
								<h3>Add an order</h3>
								<form onSubmit={submitOrder}>
									<select
										name="productId"
										onChange={handleOrderChange}
										value={order.productId}
										className="order-dropdown"
									>
										<option value="">Choose a product</option>
										{products &&
											products.map((product) => (
												<option key={product.id} value={product.id}>
													{product.name}
												</option>
											))}
									</select>

									<select
										name="clientId"
										onChange={handleOrderChange}
										value={order.clientId}
										className="order-dropdown"
									>
										<option value="">Choose a client</option>
										{clients &&
											clients.map((client) => (
												<option key={client.id} value={client.id}>
													{client.firstName} {client.lastName}
												</option>
											))}
									</select>

									<input
										type="text"
										name="quantity"
										placeholder="Amount"
										value={order.quantity}
										onChange={handleOrderChange}
									/>

									<input
										type="text"
										name="comments"
										placeholder="Additional comments (optional)"
										value={order.comments}
										onChange={handleOrderChange}
									></input>

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
						</div>
					</div>
				</div>
			)}
		</div>
	);
};

export default Home;
