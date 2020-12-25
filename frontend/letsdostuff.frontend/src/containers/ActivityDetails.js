import React, { useState } from "react";
import Card from "react-bootstrap/Card";
import Button from "react-bootstrap/Button";
import "./ActivityDetails.css";
import axios from 'axios';
import { useParams } from "react-router-dom";

function ActivityDetails() {
  const [activity, setActivity] = useState([]);
  let { id } = useParams();

  function filterActivities() {
    if(activity.length==0){
        axios.get('https://localhost:8081/api/activities/' + id, )
            .then(resp => {
                console.log("activities", resp.data);
                setActivity(resp.data);
            })
            .catch(err => console.log(err));
    }
  }
  
  filterActivities();
  return(
    <div className="ActivityDetails">
        <ActivityCard activity={activity} />
    </div>
  );
}

function ActivityCard({ activity }) {
    const [creator, setCreator] = useState([]);
    const [tags] = useState([]);
    if(activity.creator !== undefined && creator.length === 0){
        setCreator(activity.creator);
    }

    if(activity.tags !== undefined && tags.length === 0){
        activity.tags.map(tag => tags.push(tag))
    }

    return (
      <div className="ActivityDetails" key={activity.id}>
          <Card>
            <Card.Body>
                <Card.Title>{activity.name}</Card.Title>
                <Card.Text>
                {activity.description}
                <br />
                {tags.map(tag => tag + " ")}
                </Card.Text>
                <Button href = "/activities" variant="primary">Back</Button>
            </Card.Body>
            <Card.Footer>{activity.dateStart}{creator.name}</Card.Footer>
          </Card>
      </div>
    );
  }

  export default ActivityDetails;