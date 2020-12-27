import React, {useState} from "react";
import Routes from "./Routes";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { AppContext } from "./libs/contextLib";
import { LinkContainer } from "react-router-bootstrap";
import "./App.css";

function App() {
  const [authToken, setAuthToken] = useState(localStorage.getItem('authorization'));
  document.body.style.backgroundColor = '#B0E0E6';
  
  function handleLogout() {
    setAuthToken('');
    localStorage.setItem('authorization', '')
  }

  return (
    <div className="App container py-3">
      <Navbar collapseOnSelect bg="light" expand="md" className="mb-3">
        <LinkContainer to="/">
          <Navbar.Brand className="font-weight-bold ">
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
              <>
                <LinkContainer to="/settings">
                  <Nav.Link>Settings</Nav.Link>
                </LinkContainer>
                <Nav.Link onClick={handleLogout}>Logout</Nav.Link>
              </>
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
    </div>
  );
}

export default App;