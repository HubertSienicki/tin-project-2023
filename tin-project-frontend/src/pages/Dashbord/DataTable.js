import React, { useEffect, useState } from "react";
//import { deleteRecord, fetchRecords } from "../../api";
import "../../assets/styles/styles.css";
const DataTable = () => {
	const [records, setRecords] = useState([]);
	const [selectedRecord, setSelectedRecord] = useState(null);

	// useEffect(() => {
	// 	const loadData = async () => {
	// 		const response = await fetchRecords();
	// 		setRecords(response.data);
	// 	};

	// 	loadData();
	// }, []);

	const handleDelete = async () => {
		// if (selectedRecord) {
		// 	await deleteRecord(selectedRecord);
		// 	setRecords(records.filter((record) => record.id !== selectedRecord));
		// 	setSelectedRecord(null);
		// }
	};

	return (
		<div className="data-table">
			<table>
				<thead>
					<tr>
						<th>ID</th>
						<th>Nazwa</th>
						<th>Email</th>
						<th>Akcje</th>
					</tr>
				</thead>
				<tbody>
					<tr key="test">
						<td>test</td>
						<td>test</td>
						<td>test</td>
						<td>
							{/* <button onClick={() => onSelectRecord(record)}>Edytuj</button>
								<button onClick={() => onDeleteRecord(record.id)}>Usuń</button> */}
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	);
};

export default DataTable;
