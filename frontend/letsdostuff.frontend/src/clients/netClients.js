import React, { Component } from 'react';
import HubConnection from '@aspnet/signalr';

class NetClient extends Component {
    componentDidMount = () => {
        const hubConnection = new HubConnection('https://localhost:8081/SampleHub');

        this.setState((hubConnection), () =>{
            this.state.hubConnection.Start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));
        });

        this.state.hubConnection.on('Notify', message => {
            console.log(message);
        })
    }
}

export default NetClient;