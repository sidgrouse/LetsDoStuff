import React, { Component } from 'react';
import * as signalR from "@aspnet/signalr";
import Toast from 'react-bootstrap/Toast';

class Notifier extends Component {
  constructor(props){
    super(props);

    this.state = {
      headMessage: '',
      mainMessage: '',
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

      this.state.hubConnection.on('NotifyAboutParticipation', (userName, receivedMessage) => {
        this.setState({headMessage: "👋 Hello," +  userName});
        this.setState({mainMessage: receivedMessage});
        this.setState({showA: true});
      });

      this.state.hubConnection.on('NotifyAboutAcception', (userName, receivedMessage) => {
        this.setState({headMessage: "🥳 Hello, " + userName});
        this.setState({mainMessage: receivedMessage});
        this.setState({showA: true});
      });

      this.state.hubConnection.on('NotifyAboutRejection', (userName, receivedMessage) => {
        this.setState({headMessage: "😢 Hello, " + userName});
        this.setState({mainMessage: receivedMessage});
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
            <strong className="mr-auto">{this.state.headMessage}</strong>
          </Toast.Header>
          <Toast.Body> {this.state.mainMessage} </Toast.Body>
        </Toast>
      </div>
    );
  }
}

export default Notifier;