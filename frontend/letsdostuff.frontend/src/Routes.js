import React from "react";
import { Route, Switch } from "react-router-dom";
import Home from "./containers/Home";
import Login from "./containers/Login";
import Registration from "./containers/Registration";
import Activities from "./containers/Activities";
import ActivityDetails from "./containers/ActivityDetails";
import Participations from "./containers/Participations";
import UserSettings from "./containers/UserSettings";
import UserProfile from "./containers/UserProfile";
import CreateActivity from "./containers/CreateActivity";

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
      <Route exact path="/activities/create">
        <CreateActivity />
      </Route>
      < Route path="/activities/:id" children={<ActivityDetails />} />
      <Route exact path="/participations">
        <Participations />
      </Route>
      <Route exact path="/settings">
        <UserSettings />
      </Route>
      < Route path="/:profilelink" children={<UserProfile />} />
    </Switch>
  );
}