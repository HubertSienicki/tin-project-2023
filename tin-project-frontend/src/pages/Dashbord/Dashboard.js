import React from "react";
import { Link } from "react-router-dom"; // Import Link or NavLink
import "../../assets/styles/styles.css";
import DataTable from "./DataTable";

const Dashboard = () => {
	return (
		<div className="container">
			<div className="form-box">
				<nav className="navbar">
					<Link to="/" className="nav-link">
						Home
					</Link>{" "}
				</nav>
				<h2 className="form-title">Order dashboard</h2>
				<div className="dashboard-content">
					<p className="center-text">
						Here you can edit the actual orders in the database.
					</p>
					<p className="center-text">
						To edit, simply edit one of the editable fields and click submit
						changes. The page will then reload with your new changes!
					</p>
					{<DataTable />}
				</div>
			</div>
		</div>
	);
};

export default Dashboard;
