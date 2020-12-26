import React, {useState} from "react";
import Routes from "./Routes";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { AppContext } from "./libs/contextLib";
import { LinkContainer } from "react-router-bootstrap";
import Notifier from './clients/Notifier'
import "./App.css";

function App() {

  const [authToken, setAuthToken] = useState('');

  function handleLogout() {
    setAuthToken('');
  }

  return (
    <div className="App container py-3">
      <Navbar collapseOnSelect bg="light" expand="md" className="mb-3">
        <LinkContainer to="/">
          <Navbar.Brand className="font-weight-bold text-muted">
            LetsDoStuff
          </Navbar.Brand>
        </LinkContainer>
        <Navbar.Toggle />
       
        <Navbar.Collapse className="mr-auto">
          <Nav activeKey={window.location.pathname}>
            {authToken ? (
              <>
                <LinkContainer to="/activities">
                  <Nav.Link>Activities</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/participations">
                  <Nav.Link>Participations</Nav.Link>
                </LinkContainer>
              </>
            ) : (
              <></>
            )}
          </Nav>
        </Navbar.Collapse>

        <Navbar.Collapse className="justify-content-end">
          <Nav activeKey={window.location.pathname}>
            {authToken ? (
              <LinkContainer to="/">
                <Nav.Link onClick={handleLogout}>Logout</Nav.Link>
              </LinkContainer>
            ) : (
              <>
                <LinkContainer to="/signup">
                  <Nav.Link>Signup</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/login">
                  <Nav.Link>Login</Nav.Link>
                </LinkContainer>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <AppContext.Provider value={{ authToken, setAuthToken }}>
        <Routes />
      </AppContext.Provider>
      {authToken ? (
      <Notifier token={authToken} />)
      :(
        <></>
      )}
    </div>
  );
}

export default App;