import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import reducers from '../reducers';

const logger = (store) => (next) => (action) => {

  if(typeof action !== "function") {
    console.log('dispatching:', action);

    let time = performance.now();
    let actionDone = next(action);

    console.log(`Time spent: ${performance.now() - time}ms`);

    return actionDone;
  }

  return next(action);
};

const store = createStore(
  reducers,
  applyMiddleware(logger, thunk)
);

export default store;