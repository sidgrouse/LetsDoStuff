import React, { useState } from "react";
import Button from "react-bootstrap/Button";
import "./Participations.css";
import axios from "axios";
import Jumbotron from 'react-bootstrap/Jumbotron'
import "./CommonStyles.css";
import {ActivityCard} from "./ActivityCard";
import { Link } from "react-router-dom";

function Participations() {
  const [activities, setActivities] = useState([]);
  let errorStatus;

  function filterActivities() {
    if (activities.length === 0) {
      axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
      axios.get("https://localhost:8081/api/participation")
      .then((resp) => {
        setActivities(resp.data);
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
    <div className="Activities">
      <Jumbotron className="JumbotronStyle">
        {activities.map((activity) => (
          <>
            {ActivityCard({activity}, ()=>(
              <>
                <Link to={"/activities/" + activity.id}>
                  <Button variant="outline-info">Details</Button>
                </Link>
              </>
            ))}
          </>
        ))}
      </Jumbotron>
    </div>
  );
}

export default Participations;
