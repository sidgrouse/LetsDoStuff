import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import { useAppContext } from "../libs/contextLib";
import Button from "react-bootstrap/Button";
import "./Login.css";
import axios from 'axios';
import {notifyError, notifySuccess} from "./StatusNotification";

export default function Login() {
  const { setAuthToken } = useAppContext();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  function validateForm() {
    return email.length > 0 && password.length > 0;
  }

  function handleSubmit(e) {
    e.preventDefault();

    axios.post('https://localhost:8081/api/account/login', { login: email, password: password})
      .then(resp => {
        setAuthToken(resp.data.access_token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${resp.data.access_token}`;
        console.log("logged in");
        notifySuccess()
      })
      .catch(err => {
        console.log(err);
        notifyError('Incorrect login or password.')
      });
      
  }

  return (
    <div className="Login">
      <Form onSubmit={handleSubmit}>
        <Form.Group size="lg" controlId="email">
          <Form.Label>Email</Form.Label>
          <Form.Control
            autoFocus
            type="email"
            name="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </Form.Group>
        <Form.Group size="lg" controlId="password">
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            value={password}
            name="password"
            onChange={(e) => setPassword(e.target.value)}
          />
        </Form.Group>
        <Button block size="lg" type="submit" disabled={!validateForm()}>
          Login
        </Button>
      </Form>
    </div>
  );
}