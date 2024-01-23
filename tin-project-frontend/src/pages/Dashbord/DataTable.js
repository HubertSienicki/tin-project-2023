import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";

const DataTable = () => {
	const [records, setRecords] = useState([]);
	const [editedRecords, setEditedRecords] = useState({});
	const [products, setProducts] = useState([]);
	const [userRole, setUserRole] = useState("");

	const navigate = useNavigate();

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
		navigate("/login"); // Redirect to login page or home page
	};

	useEffect(() => {
		const token = sessionStorage.getItem("token");
		if (token) {
			const payload = decodeToken(token);
			const roleKey =
				"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			const role = payload[roleKey];
			setUserRole(role);

			if (role !== "Admin") {
				navigate("/unauthorized"); // redirect to unauthorized access page
			}
		} else {
			navigate("/unauthorized"); // redirect to login if no token
		}

		const fetchProducts = async () => {
			try {
				const response = await axios.get("http://localhost:5002/Product", {
					headers: {
						Authorization: `Bearer ${token}`,
					},
				});
				setProducts(response.data);
			} catch (error) {
				console.error(error);
			}
		};

		fetchProducts();

		const fetchData = async () => {
			try {
				const response = await axios.get(
					"http://localhost:5004/Order/Details",
					{
						headers: {
							Authorization: `Bearer ${token}`,
						},
					}
				);

				// Transforming the data to a suitable format
				const transformedData = response.data.map((item) => ({
					order_id: item.order.id,
					product_name: item.product.map((p) => p.name).join(", "),
					quantity: item.quantity,
					// Calculate the total price for each product and sum them up
					price: item.product
						.reduce((total, prod) => total + prod.price * item.quantity, 0)
						.toFixed(2),
					additional_comments: item.additionalColumn,
					client_name: item.order.client.firstName,
					client_lastname: item.order.client.lastName,
					client_phone_number: item.order.client.phone,
				}));
				setRecords(transformedData);
			} catch (error) {
				console.error("Error fetching data:", error);
			}
		};

		fetchData();
	}, []);

	const handleEdit = (order_id, field, value) => {
		setEditedRecords({
			...editedRecords,
			[order_id]: {
				...editedRecords[order_id],
				[field]: value,
			},
		});
	};

	const handleProductChange = (order_id, selectedProductId) => {
		const selectedProduct = products.find(
			(p) => p.id === parseInt(selectedProductId)
		);
		if (selectedProduct) {
			setEditedRecords({
				...editedRecords,
				[order_id]: {
					...editedRecords[order_id],
					product_name: selectedProduct.name,
					// Update the price calculation if needed
				},
			});
		}
	};

	const handleSubmit = async () => {
		// Here you would send the editedRecords to your API endpoint
		// For each record in editedRecords, call the API to update the record
		// Example: await updateRecord(order_id, editedRecord[order_id]);
		console.log("Submitted", editedRecords);
	};

	return (
		<div className="data-table">
			<button onClick={logout} className="button mt-20">
				Logout
			</button>
			<table>
				<thead>
					<tr>
						<th>Order ID</th>
						<th>Product Name</th>
						<th>Quantity</th>
						<th>Price</th>
						<th>Additional Comments</th>
						<th>Client Name</th>
						<th>Client Lastname</th>
						<th>Client Phone Number</th>
					</tr>
				</thead>
				<tbody>
					{records.map((record) => (
						<tr key={record.order_id}>
							<td>{record.order_id}</td>
							<td>
								<select
									value={
										editedRecords[record.order_id]?.product_name ??
										record.product_name
									}
									onChange={(e) =>
										handleProductChange(record.order_id, e.target.value)
									}
								>
									{products.map((product) => (
										<option key={product.id} value={product.id}>
											{product.name}
										</option>
									))}
								</select>
							</td>
							<td>
								<input
									type="number"
									value={
										editedRecords[record.order_id]?.quantity ?? record.quantity
									}
									onChange={(e) =>
										handleEdit(record.order_id, "quantity", e.target.value)
									}
								/>
							</td>
							<td>{record.price}</td>
							<td>
								<input
									type="text"
									value={
										editedRecords[record.order_id]?.additional_comments ??
										record.additional_comments
									}
									onChange={(e) =>
										handleEdit(
											record.order_id,
											"additional_comments",
											e.target.value
										)
									}
								/>
							</td>
							<td>{record.client_name}</td>
							<td>{record.client_lastname}</td>
							<td>{record.client_phone_number}</td>
						</tr>
					))}
				</tbody>
			</table>
			<button onClick={handleSubmit}>Submit Changes</button>
		</div>
	);
};

export default DataTable;
