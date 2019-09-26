import React from 'react';
import { Header, Icon, List } from 'semantic-ui-react'
import './App.css';
import axios from 'axios';

class App extends React.Component {

  state = {
    values: []
  }

  componentDidMount() {
    axios.get("http://localhost:5000/api/values")
      .then((response) => {
        this.setState(
          {
            values: response.data
          })
      });
  }

  render() {
    return (
      <div>
        <Header as='h2'>
          <Icon name='users' />
          <Header.Content>Reactivities</Header.Content>
        </Header>
        <List>
          {this.state.values.map((value: any) => (
            <List.Item key={value.id}>
              <List.Icon name='map' />
              <List.Content>{value.name}</List.Content>
            </List.Item>
          ))}
        </List>



      </div>
    );
  }
}

export default App;
