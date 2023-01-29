grammar Command;

command: commandSegments flags;

commandSegments: commandSegment+;

commandSegment: (STRING | IDENT)+;

flags: flag*;

flag: flagFullname | flagShortenName;

flagFullname: DoubleDash IDENT ('=' (IDENT | STRING))?;

flagShortenName: SingleDash IDENT;

STRING : '"' ('""'|~'"')* '"';
IDENT: [0-9a-zA-Z][0-9a-zA-Z-]*;

DoubleDash: '--';
SingleDash: '-';

WS: [ \t\r\n] -> skip;
