import React, { Component } from 'react';
import * as signalR from "@aspnet/signalr";
import Toast from 'react-bootstrap/Toast';

class Notifier extends Component {
  constructor(props){
    super(props);

    this.state = {
      nick: '',
      message: '',
      token: '',
      showA: false,
      hubConnection: null,
    };
  }

  componentDidMount = () =>{
    this.setState({token: this.props.token});
    let hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:8081/ParticipationHub", { accessTokenFactory: () => this.state.token})
    .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => console.log('Error while establishing connection :('));

      this.state.hubConnection.on('Notify', (userName, receivedMessage) => {
        this.setState({nick: userName});
        this.setState({message: receivedMessage});
        this.setState({showA: true});
      });
    });
  }

  render(){
    return(
      <div>
        <Toast show={this.state.showA} onClose={() => this.setState({showA: false})} delay={10000} autohide>
          <Toast.Header>
            <img src="holder.js/20x20?text=%20" className="rounded mr-2" alt="" />
            <strong className="mr-auto">👋 Hello, {this.state.nick}</strong>
          </Toast.Header>
          <Toast.Body> {this.state.message} </Toast.Body>
        </Toast>
      </div>
    );
  }
}

export default Notifier;