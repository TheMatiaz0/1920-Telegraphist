import mido
import json
import sys

_, in_file, out_file, *_ = sys.argv

mid = mido.MidiFile(in_file)

print(mid)

notes_dict = {
    93: 0,
    40: 0,
}

time = 0

tracks = {}

last_msg_time = 0

for msg in mid:
    # if msg.is_meta: 
    #     continue

    print(msg)
    
    time += msg.time

    if msg.type == 'note_off':
        if msg.note not in notes_dict:
            continue

        key = notes_dict[msg.note]
        if key not in tracks:
            tracks[key] = []
        
        tracks[key].append({'startTime': time - msg.time, 'startDeltaTime': last_msg_time, 'duration': msg.time}) # 'type': notes_dict[msg.note], 

    last_msg_time = msg.time

with open(out_file, 'w') as f:
    json.dump({'tracks': tracks}, f, indent=2)

print(time)