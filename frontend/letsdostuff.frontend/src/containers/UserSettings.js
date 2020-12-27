import React, { useState } from "react";
import "./UserSettings.css";
import "./CommonStyles.css";
import axios from "axios";
import Jumbotron from 'react-bootstrap/Jumbotron'
import Card from "react-bootstrap/Card";
import Container from 'react-bootstrap/Container'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import { Link } from "react-router-dom";

function UserSettings() {
  const [settings, setSettings] = useState([]);
  let errorStatus;

  function filterSettings() {
    if (settings.length === 0) {
      axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
      axios.get("https://localhost:8081/api/users/settings")
      .then((resp) => {
        setSettings(resp.data);
      })
      .catch(err => {
        console.log(err.response);
        errorStatus = `${err.response.status}`;

        if(errorStatus === '401'){
          localStorage.setItem('authorization', '');
          window.location.assign("/login");
        }
      });
    }
  }

  filterSettings();
  return (
    <div>
      <Jumbotron className="JumbotronStyle">
      <Card className="UserSettings">
        <Card.Body>
          <Container className="Container">
          <Row>
            <Col>First name:</Col>
            <Col>{settings.firstName}</Col>
          </Row>
          <Row>
            <Col>Last name:</Col>
            <Col>{settings.lastName}</Col>
          </Row>
          <Row>
            <Col>Email:</Col>
            <Col>{settings.email}</Col>
          </Row>
          <Row>
            <Col>Date of birth:</Col>
            <Col>{settings.dateOfBirth}</Col>
          </Row>
          <Row>
            <Col>Bio:</Col>
            <Col>{settings.bio}</Col>
          </Row>
          <Row>
            <Col>Date of registration:</Col>
            <Col>{settings.dateOfRegistration}</Col>
          </Row>
          <Row>
            <Col>Profile link:</Col>
            <Col>
              <Link to={"/" + settings.profileLink}>
                {"http://localhost:3000/"+settings.profileLink}
              </Link>
            </Col>
          </Row>
        </Container>
        </Card.Body>
        <Card.Footer >
        </Card.Footer>
      </Card>
      </Jumbotron>
    </div>
  );
}

export default UserSettings;