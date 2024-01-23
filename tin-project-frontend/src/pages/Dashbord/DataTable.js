import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/styles/styles.css";

const DataTable = () => {
	const [records, setRecords] = useState([]);
	const [currentPage, setCurrentPage] = useState(1);
	const [recordsPerPage, setRecordsPerPage] = useState(10);
	const [editedRecords, setEditedRecords] = useState({});
	const [products, setProducts] = useState([]);
	const [userRole, setUserRole] = useState("");
	const [selectedRows, setSelectedRows] = useState({});
	const [isDeleting, setIsDeleting] = useState(false); // To track if deletion is in progress

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
		if (!token) {
			navigate("/unauthorized");
			return;
		}

		const payload = decodeToken(token);
		const roleKey =
			"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
		const role = payload[roleKey];
		setUserRole(role);

		if (role !== "Admin") {
			navigate("/unauthorized");
			return;
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
					`http://localhost:5004/Order/details?pageNumber=${currentPage}&pageSize=${recordsPerPage}`,
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
				if (error.response) {
					// The request was made and the server responded with a status code
					// that falls out of the range of 2xx
					console.error("Error data:", error.response.data);
					console.error("Error status:", error.response.status);
					console.error("Error headers:", error.response.headers);
				} else if (error.request) {
					// The request was made but no response was received
					console.error("Error request:", error.request);
				} else {
					// Something happened in setting up the request that triggered an Error
					console.error("Error", error.message);
				}
				console.error("Error config:", error.config);
			}
		};

		fetchData();
		// after change trigger one of those
	}, [currentPage, recordsPerPage, navigate]);

	const handleEdit = (order_id, field, value) => {
		setEditedRecords({
			...editedRecords,
			[order_id]: {
				...editedRecords[order_id],
				[field]: value,
			},
		});
	};

	const handleSelectRow = (orderId, isSelected) => {
		setSelectedRows({
			...selectedRows,
			[orderId]: isSelected,
		});
	};

	const handleSelectAll = (isSelected) => {
		const newSelectedRows = {};
		if (isSelected) {
			records.forEach((record) => {
				newSelectedRows[record.order_id] = true;
			});
		}
		setSelectedRows(newSelectedRows);
	};

	const handleDeleteSelected = async () => {
		setIsDeleting(true);

		const token = sessionStorage.getItem("token");
		const selectedOrderIds = Object.keys(selectedRows).filter(
			(orderId) => selectedRows[orderId]
		);

		try {
			const deletePromises = selectedOrderIds.map((orderId) =>
				axios
					.delete(`http://localhost:5004/Order/details/${orderId}`, {
						headers: {
							Authorization: `Bearer ${token}`,
						},
					})
					.then((response) => {
						console.log(`Delete success for order ${orderId}:`, response);
					})
					.catch((error) => {
						console.error(`Error deleting order ${orderId}:`, error);
						throw error; // Re-throw error to be caught by the outer catch block
					})
			);

			await Promise.all(deletePromises);

			setRecords((prevRecords) =>
				prevRecords.filter(
					(record) => !selectedOrderIds.includes(record.order_id)
				)
			);
			console.log("Successfully deleted selected orders");
		} catch (error) {
			console.error("Error deleting orders:", error);
		}

		setIsDeleting(false);
		window.location.reload();
	};

	const handleSubmit = async () => {
		const token = sessionStorage.getItem("token");

		const updateRecord = async (orderId, data) => {
			try {
				const requestBody = {
					quantity: data.quantity ? parseInt(data.quantity, 10) : 0,
					additionalColumn: data.additional_comments || "",
				};

				await axios.put(
					`http://localhost:5004/Order/details/update/${orderId}`,
					requestBody,
					{
						headers: {
							Authorization: `Bearer ${token}`,
						},
					}
				);

				console.log(`Successfully updated order ${orderId}`);
			} catch (error) {
				console.error(`Error updating order ${orderId}:`, error);
			}
		};

		const updateRequests = Object.entries(editedRecords).map(
			([orderId, data]) => updateRecord(orderId, data)
		);

		await Promise.all(updateRequests);

		console.log("All updates completed, refreshing page.");

		window.location.reload();
	};

	const handlePageChange = (newPage) => {
		setCurrentPage(newPage);
	};

	const handleRecordsPerPageChange = (event) => {
		setRecordsPerPage(event.target.value);
		setCurrentPage(1); // Reset to first page when records per page changes
	};

	return (
		<div className="data-table">
			<button
				onClick={() => {
					setIsDeleting(true); // Set deletion mode
					handleDeleteSelected(); // Call the deletion function
				}}
				className="button mt-20"
				disabled={isDeleting} // Disable the button during deletion
			>
				Delete Selected
			</button>
			<button onClick={logout} className="button mt-20">
				Logout
			</button>
			<table>
				<thead>
					<tr>
						<th>
							<input
								type="checkbox"
								onChange={(e) => handleSelectAll(e.target.checked)}
							/>
						</th>
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
							<td>
								<input
									type="checkbox"
									checked={selectedRows[record.order_id] || false}
									onChange={(e) =>
										handleSelectRow(record.order_id, e.target.checked)
									}
								/>
							</td>
							<td>{record.order_id}</td>
							<td>{record.product_name}</td>
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
			<div className="pagination-controls">
				<button
					onClick={() => handlePageChange(currentPage - 1)}
					disabled={currentPage === 1}
				>
					Previous
				</button>
				<span>Page {currentPage}</span>
				<button onClick={() => handlePageChange(currentPage + 1)}>Next</button>
				<select value={recordsPerPage} onChange={handleRecordsPerPageChange}>
					<option value={1}>1</option>
					<option value={10}>10</option>
					<option value={20}>20</option>
					<option value={50}>50</option>
				</select>
			</div>
			<button onClick={handleSubmit}>Submit Changes</button>
		</div>
	);
};

export default DataTable;
