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
import {useParams} from "react-router-dom";

function UserProfile() {
  const [profile, setProfile] = useState([]);
  let errorStatus;
  let { profilelink } = useParams();

  function filterActivities() {
    if (profile.length === 0) {
      axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
      axios.get("https://localhost:8081/api/users/" + profilelink)
      .then((resp) => {
        setProfile(resp.data);
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

  filterActivities();
  return (
    <div>
      <Jumbotron className="JumbotronStyle">
      <Card className="UserSettings">
        <Card.Body>
          <Container className="Container">
          <Row>
          <Card.Title>{profile.contactName}</Card.Title>
          </Row>
          <Row>
            <Col>Email:</Col>
            <Col>{profile.email}</Col>
          </Row>
          <Row>
            <Col>Date of birth:</Col>
            <Col>{profile.dateOfBirth}</Col>
          </Row>
          <Row>
            <Col>Bio:</Col>
            <Col>{profile.bio}</Col>
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

export default UserProfile;