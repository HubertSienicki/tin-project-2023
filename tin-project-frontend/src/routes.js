import React from "react";
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";

const Routes = () => (
	<Router>
		<Switch>
			<Route exact path="/" component={Home} />
			<Route path="/login" component={Login} />
			<Route path="/register" component={Register} />
			<Route path="/dashboard" component={Dashboard} />
			{/*more routes here if needed*/}
		</Switch>
	</Router>
);

export default Routes;
