import React from "react";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Dashboard from "./pages/Dashbord/Dashboard";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";
import UnauthorizedPage from "./pages/Unauthorized/Unauthorized";

const AppRoutes = () => (
	<Router>
		<Routes>
			{<Route exact path="/" element={<Home />} />}
			<Route path="/login" element={<Login />} />
			<Route path="/register" element={<Register />} />
			<Route path="/dashboard" element={<Dashboard />} />
			<Route path="/unauthorized" element={<UnauthorizedPage />} />
			{/* more routes here if needed */}
		</Routes>
	</Router>
);

export default AppRoutes;
