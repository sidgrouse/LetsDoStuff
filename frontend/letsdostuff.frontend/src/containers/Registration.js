import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import { useAppContext } from "../libs/contextLib";
import Button from "react-bootstrap/Button";
import "./Registration.css";
import axios from 'axios';
import {notifyError, notifySuccess} from "./StatusNotification";

export default function Registration() {
  const { setAuthToken } = useAppContext();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  //const [dateOfBirth, setDateOfBirth] = useState("");
  const [bio, setBio] = useState("");

  function validateForm() {
    return firstName.length > 0 && password.length > 0;
  }
/*firstName*	string
lastName*	string
email	string($email)nullable: true
password*	string
confirmPassword	string nullable: true
dateOfBirth	string($date-time) nullable: true
bio	string*/


  function handleSubmit(e) {
    e.preventDefault();

    axios.post('https://localhost:8081/api/users/registration', { firstName, lastName, email, password, confirmPassword, bio })
      .then(resp => {
        console.log("registered", resp);
        notifySuccess();
      })
      .catch(err => {
        console.log(err);
        console.log(err.response);
        notifyError();
      });
      
  }

  return (
    <div className="Reg">
      <Form onSubmit={handleSubmit}>
        <Form.Group size="lg" controlId="email">
          <Form.Label>Email</Form.Label>
          <Form.Control autoFocus type="email" value={email} onChange={(e) => setEmail(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="firstName">
          <Form.Label>First Name</Form.Label>
          <Form.Control type="firstName" value={firstName} onChange={(e) => setFirstName(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="lastName">
          <Form.Label>Last Name</Form.Label>
          <Form.Control type="lastName" value={lastName} onChange={(e) => setLastName(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="password">
          <Form.Label>Password</Form.Label>
          <Form.Control type="password" value={password} onChange={(e) => setPassword(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="confirmPassword">
          <Form.Label>Confirm password</Form.Label>
          <Form.Control type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="bio">
          <Form.Label>Bio</Form.Label>
          <Form.Control as="textarea" rows={4} type="bio" value={bio} onChange={(e) => setBio(e.target.value)}
          />
        </Form.Group>
        <Button block size="lg" type="submit" disabled={!validateForm()}>
          Submit
        </Button>
      </Form>
    </div>
  );
}