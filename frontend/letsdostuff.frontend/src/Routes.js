import React from "react";
import { Route, Switch } from "react-router-dom";
import Home from "./containers/Home";
import Login from "./containers/Login"
import Registration from "./containers/Registration";
import Activities from "./containers/Activities";
import ActivityDetails from "./containers/ActivityDetails"

export default function Routes() {
  return (
    <Switch>
      <Route exact path="/">
        <Home />
      </Route>
      <Route exact path="/login">
        <Login />
      </Route>
      <Route exact path="/signup">
        <Registration />
      </Route>
      <Route exact path="/activities">
        <Activities />
      </Route>
      <Route path="/activities/:id" children={<ActivityDetails />}>
            </Route>
    </Switch>
  );
}