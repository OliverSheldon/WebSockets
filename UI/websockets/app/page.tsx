"use client"

import { useEffect, useState } from "react"

export default function Home() {
  let [stream, updateStream] = useState<String>();
  let [status, updateStatus] = useState<String>("Disconnected");

  const websocket = new WebSocket(`${process.env.NEXT_PUBLIC_WEB_SOCKET_ENDPOINT}/ws`);

  websocket.onmessage = (e) => {
    updateStream(e.data);
  }

  websocket.onopen = (e) => {
    updateStatus("Connected");
    websocket.send("Hello Server");
  };

  websocket.onclose = (e) => {
    updateStatus("Disconnected");
  };

  return (
    <div>
      <h1>Hello World</h1>
      <h2>Status:</h2>
      <div>
        <p>{status}</p>
      </div>
      <h2>Messages:</h2>
      <div>
        {/* {stream.map((message, i)=> */}
            <p>{stream}</p>
      {/* )} */}
      </div>
    </div>
  );
}