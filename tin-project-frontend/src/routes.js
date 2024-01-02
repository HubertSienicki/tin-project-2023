import React from "react";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
// import Dashboard from "./pages/Dashboard";
// import Home from "./pages/Home";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";

const AppRoutes = () => (
	<Router>
		<Routes>
			{/* <Route exact path="/" component={Home} /> */}
			<Route path="/login" element={<Login />} />
			<Route path="/register" element={<Register />} />
			{/* <Route path="/dashboard" component={Dashboard} /> */}
			{/*more routes here if needed*/}
		</Routes>
	</Router>
);

export default AppRoutes;
