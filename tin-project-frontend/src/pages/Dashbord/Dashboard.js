import React from "react";
import "../../assets/styles/styles.css";
import DataTable from "./DataTable";

const Dashboard = () => {
	return (
		<div className="container">
			<div className="form-box">
				<h2 className="form-title">Dashboard zamówień</h2>
				<div className="dashboard-content">
					<p>Tutaj możesz edytować aktualne zamówienia</p>
					{<DataTable />}
				</div>
			</div>
		</div>
	);
};

export default Dashboard;
