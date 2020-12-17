import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import { useAppContext } from "../libs/contextLib";
import Button from "react-bootstrap/Button";
import "./Login.css";
import axios from 'axios';

export default function Activities() {
  const { setAuthToken } = useAppContext();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  function filterActivities() {

    axios.get('https://localhost:8081/api/activity/all', )
      .then(resp => {
        console.log("activities", resp);
      })
      .catch(err => console.log(err));
      
  }

  return (
    <div className="Activities">
    </div>
  );
}