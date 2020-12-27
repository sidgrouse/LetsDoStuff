import React, { useState } from "react";
import Button from "react-bootstrap/Button";
import "./ActivityDetails.css";
import "./CommonStyles.css";
import axios from "axios";
import {useParams} from "react-router-dom";
import Jumbotron from 'react-bootstrap/Jumbotron'
import {ActivityCard} from "./ActivityCard";

function ActivityDetails() {
  const [activity, setActivity] = useState([]);
  const [participations, setParticipations] = useState([]);
  let { id } = useParams();
  const [check, setCheck] = useState(false);

  function filterActivities() {
    if (activity.length === 0) {
      axios.get("https://localhost:8081/api/activities/" + id)
      .then((resp) => {
        setActivity(resp.data);
      })
      .catch((err) => console.log(err));
    }
  }

  function checkParticipation(){
    if (!check){
      axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
      axios.get("https://localhost:8081/api/participation")
      .then((resp) => {
        setParticipations(resp.data);
      })

      setCheck(true);
    }
  }

  filterActivities();
  checkParticipation();
  if (!participations.some((participation) => participation.id === activity.id)){
    return (
      <div className="ActivityDetails">
        <Jumbotron className="JumbotronStyle">
        {ActivityCard({activity}, ()=>(
          <Button variant="outline-success" onClick={() => participate(activity.id)} className="Submit">Participate</Button>
        ))}
        </Jumbotron>
      </div>
    );
  }
  else {
    return (
      <div className="ActivityDetails">
        <Jumbotron className="JumbotronStyle">
        {ActivityCard({activity}, ()=>(
          <Button variant="outline-danger" onClick={() => notParticipate(activity.id)} className="Submit">Not participate</Button>
        ))}
        </Jumbotron>
      </div>
    );
  }
}

function participate(id) {
  let params = new URLSearchParams();
  params.append('IdActivity', id);
  let errorStatus;

  axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
  axios.post('https://localhost:8081/api/participation', params)
      .then(resp => {
        window.location.reload();
        console.log(resp.data);
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

function notParticipate(id) {
  let params = new URLSearchParams();
  params.append('IdActivity', id);
  let errorStatus;
  console.log(id)

  axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
  axios.delete('https://localhost:8081/api/participation', {data: params})
      .then(resp => {
        window.location.reload();
        console.log(resp.data);
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

export default ActivityDetails;