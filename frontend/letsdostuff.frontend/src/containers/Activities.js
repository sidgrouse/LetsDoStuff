import React, { useState } from "react";
import Card from "react-bootstrap/Card";
import Button from "react-bootstrap/Button";
import "./Activities.css";
import axios from 'axios';

function Activities() {
  const [activities, setActivities] = useState([]);

  function filterActivities() {
      if(activities.length==0){
            axios.get('https://localhost:8081/api/activities', )
                .then(resp => {
                    console.log("activities", resp.data);
                    setActivities(resp.data);
                })
                .catch(err => console.log(err));
        }
  }
  
  filterActivities();
  return(
    <div className="Activities">
        {activities.map(activity => (
            <ActivityCard activity={activity} />
        ))}
    </div>
  );
}

function ActivityCard({ activity }) {
    return (
      <div className="Activity" key={activity.id}>
          <Card>
            <Card.Body>
                <Card.Title>{activity.name}</Card.Title>
                <Card.Text>
                {activity.description}
                </Card.Text>
                <Button variant="primary">Details</Button>
            </Card.Body>
            <Card.Footer>{activity.dateStart}</Card.Footer>
          </Card>
      </div>
    );
  }

  export default Activities;