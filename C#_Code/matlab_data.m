Samples=[1:10]
Timestamps=[0:1000:9000]

Digitals=[
	 0,0,0 ;
	 0,0,1 ;
	 0,1,0 ;
	 0,1,1 ;
	 1,0,0 ;
	 1,0,1 ;
	 1,1,0 ;
	 1,1,1 ;
	 0,0,0 ;
	 0,0,1 ]

Analogs=[
	 8965, -6232, -3178, -36 ;
	 9112, -3751, -5940, -14 ;
	 8340,  -886, -8103,  12 ;
	 6749,  2061, -9475,  28 ;
	 4482,  4818, -9954,   6 ;
	 1777,  7117, -9449,   6 ;
	-1082,  8724, -8004,  43 ;
	-3857,  9466, -5767,  52 ;
	-6271,  9265, -2960,  28 ;
	-8077,  8159,   125,  -8 ]

% ASCII
fprintf('\nASCII\n')
fprintf('      <Samples>\n')
for row=1:size(Analogs,1)
    fprintf('        <Sample>\n')
    fprintf('            <Sample_Number>%d</Sample_Number>\n',Samples(row))
    fprintf('            <Timestamp>%d</Timestamp>\n',Timestamps(row))
    fprintf('            <Analog_Channel_Values>')
    for col=1:size(Analogs,2)
        if col==1
            fprintf('%d',Analogs(row,col))
        else
            fprintf(',%d',Analogs(row,col))
        end
    end
    fprintf('</Analog_Channel_Values>\n')
    fprintf('            <Digital_Channel_Values>')
    for col=1:size(Digitals,2)
        if col==1
            fprintf('%d',Digitals(row,col))
        else
            fprintf(',%d',Digitals(row,col))
        end
    end
    fprintf('</Digital_Channel_Values>\n')
    fprintf('        </Sample>\n')
end
fprintf('      </Samples>\n')


% BINARY-Base64
fprintf('\nBINARY-Base64\n')
fprintf('      <Samples>\n')
for row=1:size(Analogs,1)
    fprintf('        <Sample>\n')
    fprintf('            <Sample_Number>"%s"</Sample_Number>\n',matlab.net.base64encode(int32(Samples(row))))
    fprintf('            <Timestamp>"%s"</Timestamp>\n',matlab.net.base64encode(int32(Timestamps(row))))
    fprintf('            <Analog_Channel_Values>')
    fprintf('"%s"',matlab.net.base64encode(int16(Analogs(row,:))))
    fprintf('</Analog_Channel_Values>\n')

    accumulator=0;
    chunks=[];
    for col=1:size(Digitals,2)
        if mod(col,16)==1
            accumulator=0;
        end
        accumulator=accumulator+Digitals(row,col)*2^(15-mod(col,16));
        if mod(col,16)==0
            chunks=[chunks,accumulator];
        end
    end
    chunks=[chunks,accumulator];
    fprintf('            <Digital_Channel_Values>"%s"</Digital_Channel_Values>\n',matlab.net.base64encode(int16(chunks)))
    fprintf('        </Sample>\n')
end
fprintf('      </Samples>\n')



% BINARY32-Base64
fprintf('\nBINARY32-Base64\n')
fprintf('      <Samples>\n')
for row=1:size(Analogs,1)
    fprintf('        <Sample>\n')
    fprintf('            <Sample_Number>"%s"</Sample_Number>\n',matlab.net.base64encode(int32(Samples(row))))
    fprintf('            <Timestamp>"%s"</Timestamp>\n',matlab.net.base64encode(int32(Timestamps(row))))
    fprintf('            <Analog_Channel_Values>')
    fprintf('"%s"',matlab.net.base64encode(int32(Analogs(row,:))))
    fprintf('</Analog_Channel_Values>\n')

    accumulator=0;
    chunks=[];
    for col=1:size(Digitals,2)
        if mod(col,16)==1
            accumulator=0;
        end
        accumulator=accumulator+Digitals(row,col)*2^(15-mod(col,16));
        if mod(col,16)==0
            chunks=[chunks,accumulator];
        end
    end
    chunks=[chunks,accumulator];
    fprintf('            <Digital_Channel_Values>"%s"</Digital_Channel_Values>\n',matlab.net.base64encode(int16(chunks)))
    fprintf('        </Sample>\n') 
end
fprintf('      </Samples>\n')


% FLOAT32-Base64
fprintf('\nFLOAT32-Base64\n')
fprintf('      <Samples>\n')
for row=1:size(Analogs,1)
    fprintf('        <Sample>\n')
    fprintf('            <Sample_Number>"%s"</Sample_Number>\n',matlab.net.base64encode(int32(Samples(row))))
    fprintf('            <Timestamp>"%s"</Timestamp>\n',matlab.net.base64encode(int32(Timestamps(row))))
    fprintf('            <Analog_Channel_Values>')
    fprintf('"%s"',matlab.net.base64encode(single(Analogs(row,:))))
    fprintf('</Analog_Channel_Values>\n')

    accumulator=0;
    chunks=[];
    for col=1:size(Digitals,2)
        if mod(col,16)==1
            accumulator=0;
        end
        accumulator=accumulator+Digitals(row,col)*2^(15-mod(col,16));
        if mod(col,16)==0
            chunks=[chunks,accumulator];
        end
    end
    chunks=[chunks,accumulator];
    fprintf('            <Digital_Channel_Values>"%s"</Digital_Channel_Values>\n',matlab.net.base64encode(int16(chunks)))
    fprintf('        </Sample>\n')    
end
fprintf('      </Samples>\n')

