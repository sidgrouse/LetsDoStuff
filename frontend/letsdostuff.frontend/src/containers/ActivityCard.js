import React from "react";
import Card from "react-bootstrap/Card";
import Badge from "react-bootstrap/Badge";

export function ActivityCard({ activity }, btn) {
  const styles = {
    activity: {
      padding: '10px 0',
      textAlign: 'center',
    },
    card: {
      boxShadow: '0 1px 0 rgba(9,30,66,.25)'
    },
    headerBody: {
      display: 'flex',
      marginBottom: '.5rem',
      fontWeight: 'bold'
    },
    body: {
      backgroundColor: '#FFFFFF'
    },
    tags: {
      display: 'flex',
    },
    creator: {
      textAlign: 'right',
      marginTop: '.5rem'
    }
  };

  return (
    <div style={styles.activity} key={activity.id}>
      <Card style={styles.card}>
        <Card.Body style={styles.body}>
          <div style={styles.headerBody}>
            {activity.dateStart}
          </div>
          <Card.Title>{activity.name}</Card.Title>
          <Card.Text>{activity.description}</Card.Text>
          {btn()}
        </Card.Body>
        <Card.Footer >
          <div style={styles.tags}>
          {activity.tags ? activity.tags.map((tag) => (
            <div>
              <Badge variant="info">{tag}</Badge>&nbsp;
            </div>
          )) : ""}
          </div>
          <div style={styles.creator}>
            <span>
            {activity.creator ? activity.creator.name : ""}
            </span>
          </div>
        </Card.Footer>
      </Card>
    </div>
  );
}