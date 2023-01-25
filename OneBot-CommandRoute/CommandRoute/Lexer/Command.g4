grammar Command;

command: commandSegment flags;

commandSegment: VAL+;

flags: flag*;

flag: flagFullname | flagShortenName;

flagFullname: DoubleDash VAL ('=' VAL)?;

flagShortenName: SingleDash VAL;

VAL: [0-9a-zA-Z][0-9a-zA-Z-]*;

DoubleDash: '--';
SingleDash: '-';

WS: [ \t\r\n] -> skip;